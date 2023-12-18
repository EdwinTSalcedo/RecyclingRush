using UnityEngine;
using System.Threading.Tasks;
using System;

public class Movimiento_autonomo : MonoBehaviour
{
    private Rigidbody rb;
    private bool detenido;
    public float anguloObjetivo;
    private bool girando;
    private bool calculando;
    public float angle_predict;
    public int elec;
    public float velocidadMovimiento = 5f;
    public float velocidadAngular = 5f;
    public float rangoAnguloMinimo = 90f;
    public float rangoAnguloMaximo = 180f;
    public float toleranciaAngular = 10f;
    public bool autonomous;
    private float intelreset;
    private float angulodeseado;
    private float angulorango;
    private DateTime tiempoUltimaLlamada = DateTime.MinValue;
    private TimeSpan tiempoEspera = TimeSpan.FromSeconds(5); // Cambia el tiempo de espera según tus necesidades
    private float tiempoEspe = 1.0f; // Cambia este valor a la cantidad de segundos que desee
    private float tiempoUltimaDeteccion = 0.0f;
    private float tiempodecambio = 0.0f;
    private Vector3 direccionMovimiento;
    public modality1 intvalid1;
    public modality2 intvalid2;
    public modality3 intvalid3;
    public modality4 intvalid4;
    private bool modad4;
    private bool modad3;
    private bool modad2;
    private bool modad1;
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        detenido = false;
        girando = false;
        calculando = false; // Añadido para inicializar la variable
        autonomous = false; // Añadido para inicializar la variable
        angulodeseado = 0; // Añadido para inicializar la variable
        angulorango = 0; // Añadido para inicializar la variable
        direccionMovimiento = Vector3.zero; // Añadido para inicializar la variable
        elec = 0; // Añadido para inicializar la variable
        anguloObjetivo = 0; // Añadido para inicializar la variable
        intelreset = Time.time + 25f;
    }

    private void FixedUpdate()
    {
        angulodeseado = transform.eulerAngles.y;
        if (!detenido)
        {
            switch (elec) 
            {
                case 0:
                    MoverEnDireccionControlada();
                    autonomous = false;
                    modad3=false;
                    modad1=false;
                    modad2=false;
                    break;
                case 1:
                    MoverEnDireccionAleatoria();

                    if (Time.time>= tiempodecambio)
                    {
                        
                        modad3=false;
                        modad2=false;
                        modad4=false;
                        autonomous = true;
                        if(!modad1)
                        {
                            intvalid1.enabled = true;
                            intvalid2.enabled = false;
                            intvalid3.enabled = false;
                            intvalid4.enabled = false;
                            modad1=true;
                        }
                        if (Mathf.Abs(angulodeseado- angulorango) <= toleranciaAngular)
                        {
                            calculando = false;
                        }
                        else
                        {
                            float direccionGiro = Mathf.Sign(angulodeseado- angulorango) * -1;
                            rb.angularVelocity = transform.up * direccionGiro * velocidadAngular;
                        }
                        
                    }
                    break;
                case 2:
                    MoverEnDireccionAleatoria();

                    if (Time.time>= tiempodecambio)
                    {
                        
                        modad3=false;
                        modad1=false;
                        modad4=false;
                        autonomous = true;
                        if(!modad2)
                        {
                            intvalid1.enabled = false;
                            intvalid2.enabled = true;
                            intvalid3.enabled = false;
                            intvalid4.enabled = false;
                            modad2=true;
                        }
                        if (Mathf.Abs(angulodeseado- angulorango) <= toleranciaAngular)
                        {
                            calculando = false;
                        }
                        else
                        {
                            float direccionGiro = Mathf.Sign(angulodeseado- angulorango) * -1;
                            rb.angularVelocity = transform.up * direccionGiro * velocidadAngular;
                        }
                        
                    }
                    break;
                case 3:
                    MoverEnDireccionAleatoria();
                    if (Time.time>= tiempodecambio)
                    {
                        autonomous = true;
                        modad1=false;
                        modad2=false;
                        modad4=false;
                        if(!modad3)
                        {
                            intvalid1.enabled = false;
                            intvalid2.enabled = false;
                            intvalid3.enabled = true;
                            intvalid4.enabled = false;
                            modad3=true;
                        }
                        if (Mathf.Abs(angulodeseado- angulorango) <= toleranciaAngular)
                        {
                            calculando = false;
                        }
                        else
                        {
                            float direccionGiro = Mathf.Sign(angulodeseado- angulorango) * -1;
                            rb.angularVelocity = transform.up * direccionGiro * velocidadAngular;
                        }
                    }
                    break;
                case 4:
                    MoverEnDireccionAleatoria();
                    if (Time.time>= tiempodecambio)
                    {
                        autonomous = true;
                        modad1=false;
                        modad2=false;
                        modad3=false;
                        if(!modad4)
                        {
                            intvalid1.enabled = false;
                            intvalid2.enabled = false;
                            intvalid3.enabled = false;
                            intvalid4.enabled = true;
                            modad4=true;
                        }
                        if (Mathf.Abs(angulodeseado- angulorango) <= toleranciaAngular)
                        {
                            calculando = false;
                        }
                        else
                        {
                            float direccionGiro = Mathf.Sign(angulodeseado- angulorango) * -1;
                            rb.angularVelocity = transform.up * direccionGiro * velocidadAngular;
                        }
                    }
                    break;
                default:
                    MoverEnDireccionControlada();
                    break;
            }
        }
        else
        {
            
            GirarHaciaAnguloAleatorio();
        }
    }

    private void OnCollisionStay(Collision other) 
    {
        if (!detenido)
        {
            if (other.gameObject.CompareTag("limit"))
            {
                // Verificar si ha pasado suficiente tiempo desde la última detección
                if (Time.time - tiempoUltimaDeteccion >= tiempoEspe)
                {
                    
                    rb.velocity = transform.forward* -1 * velocidadMovimiento;
                    detenido = true;
                    tiempoUltimaDeteccion = Time.time;
                }
            }
        }
        else
        {
            // Si el tiempo de espera ha pasado, reactiva la detección
            if (Time.time - tiempoUltimaDeteccion >= tiempoEspe)
            {
                detenido = false;
                tiempoUltimaDeteccion = Time.time;
            }
        }
    }

    private void MoverEnDireccionControlada()
    {
        // Obtener las entradas del eje vertical y horizontal
        float movimientoVertical = Input.GetAxis("Vertical");
        float movimientoHorizontal = Input.GetAxis("Horizontal");

        // Calcular la dirección de movimiento y rotación 
        direccionMovimiento = transform.forward * movimientoVertical;
        Vector3 direccionRotacion = transform.up * movimientoHorizontal;

        // Asignar la velocidad de movimiento y rotación al Rigidbody
        rb.velocity = direccionMovimiento * velocidadMovimiento;
        rb.angularVelocity = direccionRotacion * velocidadAngular;
    }
    
    private void MoverEnDireccionAleatoria()
    {
        rb.velocity = transform.forward * velocidadMovimiento;
    }

    private void GirarHaciaAnguloAleatorio()
    {
        float anguloActual = transform.eulerAngles.y;
        
        if (!girando)
        {
            anguloObjetivo = UnityEngine.Random.Range(rangoAnguloMinimo, rangoAnguloMaximo);
            anguloObjetivo += anguloActual;
            
            if(anguloObjetivo>360)
            {
                anguloObjetivo -=360;
            }

            girando = true;
        }

        if (Mathf.Abs(anguloActual - anguloObjetivo) <= toleranciaAngular)
        {
            girando = false;
            detenido  = false;
        }
        else
        {
            
            float direccionGiro = 1;
            rb.angularVelocity = transform.up * direccionGiro * velocidadAngular;
        }
    }

    public void model_change(int numberlist)
    {
        tiempodecambio = Time.time + 15f;
        intvalid1.enabled = false;
        intvalid2.enabled = false;
        intvalid3.enabled = false;
        intvalid4.enabled = false;
        elec = numberlist;
    }

    public void GirarHaciaAnguloAutonoma(float anguloconduccion)
    {
        angle_predict = anguloconduccion;   
        if (autonomous)
        {
            angulorango = angulodeseado + anguloconduccion; 
            if (angulorango > 360)
            {
                angulorango -= 360;
            }

            if (angulorango < -360)
            {
                angulorango += 360;
            }
        }
    }
}
