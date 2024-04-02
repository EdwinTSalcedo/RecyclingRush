#include <stdio.h>
#include <math.h>
#include "hardware/pwm.h"

#include <rcl/rcl.h>
#include <rcl/error_handling.h>
#include <rclc/rclc.h>
#include <rclc/executor.h>

#include <geometry_msgs/msg/twist.h>
#include <rmw_microros/rmw_microros.h>

#include "pico/stdlib.h"
#include "pico_uart_transports.h"

// Macro Functions
#define constrain(amt, low, high) ((amt)<(low)?(low):((amt)>(high)?(high):(amt)))
#define RCCHECK(fn){rcl_ret_t temp_rc = fn; if((temp_rc != RCL_RET_OK)){return temp_rc;}}

// PINS
const uint LED_BUILTIN = 25;
const uint PIN_LEFT_FORWARD = 10;
const uint PIN_LEFT_BACKWARD = 11;
const uint PIN_RIGHT_FORWARD = 12;
const uint PIN_RIGHT_BACKWARD = 13;

// Enable PINS
const uint PIN_EN_LEFT_FORWARD = 6;
const uint PIN_EN_LEFT_BACKWARD = 7;
const uint PIN_EN_RIGHT_FORWARD = 8;
const uint PIN_EN_RIGHT_BACKWARD = 9;

// PWM Values
#define PWM_MOTOR_MIN 10
#define PWM_MOTOR_MAX 1000

int aux = 0;

// ROS Variables
rcl_subscription_t subscriber;
geometry_msgs__msg__Twist msg;

// Functions
void cmd_vel_callback(const void * msgin);
void timer_callback(rcl_timer_t *timer, int64_t last_call_time);
float fmap(float val, float in_min, float in_max, float out_min, float out_max);

int main()
{
	// UART MicroROS
    rmw_uros_set_custom_transport(
		true,
		NULL,
		pico_serial_transport_open,
		pico_serial_transport_close,
		pico_serial_transport_write,
		pico_serial_transport_read
	);
	
	// Wait for agent successful ping for 2 minutes.
    const int timeout_ms = 1000; 
    const uint8_t attempts = 120;
    RCCHECK(rmw_uros_ping_agent(timeout_ms, attempts));
	
	// Configuring PWM
	const uint count_top = 1000;
	
	pwm_config cfg_1 = pwm_get_default_config();
	pwm_config cfg_2 = pwm_get_default_config();
	pwm_config cfg_3 = pwm_get_default_config();
	pwm_config cfg_4 = pwm_get_default_config();
	
	pwm_config_set_wrap(&cfg_1, count_top);
	pwm_config_set_wrap(&cfg_2, count_top);
	pwm_config_set_wrap(&cfg_3, count_top);
	pwm_config_set_wrap(&cfg_4, count_top);
	
	pwm_init(pwm_gpio_to_slice_num(PIN_LEFT_FORWARD), &cfg_1, true);
	pwm_init(pwm_gpio_to_slice_num(PIN_LEFT_BACKWARD), &cfg_2, true);
	pwm_init(pwm_gpio_to_slice_num(PIN_RIGHT_FORWARD), &cfg_3, true);
	pwm_init(pwm_gpio_to_slice_num(PIN_RIGHT_BACKWARD), &cfg_4, true);
	
	gpio_set_function(PIN_LEFT_FORWARD, GPIO_FUNC_PWM);
	gpio_set_function(PIN_LEFT_BACKWARD, GPIO_FUNC_PWM);
	gpio_set_function(PIN_RIGHT_FORWARD, GPIO_FUNC_PWM);
	gpio_set_function(PIN_RIGHT_BACKWARD, GPIO_FUNC_PWM);
	
	
    // Creating Variables
    rcl_allocator_t allocator = rcl_get_default_allocator();
    rclc_support_t support;
    rcl_node_t node;
	rclc_executor_t executor;
	rcl_timer_t timer;
	
	// Create Init Options
	RCCHECK(rclc_support_init(&support, 0, NULL, &allocator));
	
	// Create Node
	RCCHECK(rclc_node_init_default(&node, "pico_diffdrive", "", &support));
	
	// Create Subscriber
	RCCHECK(rclc_subscription_init_default(
        &subscriber,
        &node,
        ROSIDL_GET_MSG_TYPE_SUPPORT(geometry_msgs, msg, Twist),
        "/cmd_vel"));
        
	// Create timer
	RCCHECK(rclc_timer_init_default(
        &timer,
        &support,
        RCL_MS_TO_NS(100),
        timer_callback));
	
	// Create Executor
	RCCHECK(rclc_executor_init(&executor, &support.context, 2, &allocator));
	RCCHECK(rclc_executor_add_subscription(&executor, &subscriber, &msg, &cmd_vel_callback, ON_NEW_DATA));
	RCCHECK(rclc_executor_add_timer(&executor, &timer));
	
	// Init LED
	gpio_init(LED_BUILTIN);
	gpio_set_dir(LED_BUILTIN, GPIO_OUT);  //// AAAAAA
	//gpio_put(LED_BUILTIN, 1);

	// Init ENABLE Pins
	gpio_init(PIN_EN_LEFT_FORWARD);
	gpio_init(PIN_EN_LEFT_BACKWARD);
	gpio_init(PIN_EN_RIGHT_FORWARD);
	gpio_init(PIN_EN_RIGHT_BACKWARD);

	// Configuring as Outputs
	gpio_set_dir(PIN_EN_LEFT_FORWARD, GPIO_OUT);
	gpio_set_dir(PIN_EN_LEFT_BACKWARD, GPIO_OUT);
	gpio_set_dir(PIN_EN_RIGHT_FORWARD, GPIO_OUT);
	gpio_set_dir(PIN_EN_RIGHT_BACKWARD, GPIO_OUT);

    while (true)
    {
        rclc_executor_spin_some(&executor, RCL_MS_TO_NS(10));
    }
    
    // Free Resources
    RCCHECK(rcl_subsccription_fini(&subscriber, &node));
    RCCHECK(rcl_node_fini(&node));
    vTaskDelete(NULL);
    
    return 0;
}

void cmd_vel_callback(const void * msgin){
	//const geometry_msgs__msg__Twist *msg = (const geometry_msgs__msg__Twist *) msgin;
	// printf("Message received: %f %f\n", msg->linear.x, msg->angular.z);
}

void timer_callback(rcl_timer_t *timer, int64_t last_call_time){
	// Flag
	gpio_put(LED_BUILTIN, aux);
	
	if(aux == 0){aux = 1;}
	else if(aux == 1){aux = 0;}
	
	// Using Linear.X and Angular.Z
	float linear = constrain(msg.linear.x, -1, 1);
    float angular = constrain(msg.angular.z, -1, 1);
    
    // Calculating Speed
    float left = (linear - angular) / 2.0f;
    float right = (linear + angular) / 2.0f;
    
    // Mapping Values
    uint16_t pwmLeft = (uint16_t) fmap(fabs(left), 0, 1, PWM_MOTOR_MIN, PWM_MOTOR_MAX);
    uint16_t pwmRight = (uint16_t) fmap(fabs(right), 0, 1, PWM_MOTOR_MIN, PWM_MOTOR_MAX);

	// Setting Enable Pins
	gpio_put(PIN_EN_LEFT_FORWARD, 1);
	gpio_put(PIN_EN_LEFT_BACKWARD, 1);
	gpio_put(PIN_EN_RIGHT_FORWARD, 1);
	gpio_put(PIN_EN_RIGHT_BACKWARD, 1);

    // Publishing PWM Values
    pwm_set_gpio_level(PIN_LEFT_FORWARD, pwmLeft * (left > 0));
    pwm_set_gpio_level(PIN_LEFT_BACKWARD, pwmLeft * (left < 0));
    pwm_set_gpio_level(PIN_RIGHT_FORWARD, pwmRight * (right > 0));
    pwm_set_gpio_level(PIN_RIGHT_BACKWARD, pwmRight * (right < 0));
    
}

float fmap(float val, float in_min, float in_max, float out_min, float out_max){
	return (val - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
}








