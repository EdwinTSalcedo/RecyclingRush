using UnityEngine;
using System.Net;
using System.Text;
public class Movimiento : MonoBehaviour
{
    public GameObject cam;
    public GameObject ang;
    public float aceleracion=50f;
    public float aceleracionangular=5f;
    public float aceleracionactual=0f;
    public float aceleracionangularactual=0f;
    public Rigidbody rb;
    public Rigidbody rbw;
    public int movHorizontal=0;
    public float movVertical=0;
    public float anguloreal=0;
    public float velocidadreal=0;
    public float anguloUI=0;
    public float aumentogiro;
    public float velocidadUI=0;
    public int region;
    private int valorx;
    private int valorz;
    public string anguloenvio;
    public string velocidadenvio;
    
    private void Start()
    {
        
        rb=GetComponent<Rigidbody>();   
        rbw=ang.GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        Vector3 rotacionActual = ang.transform.localEulerAngles;
        movVertical = Input.GetAxis("Vertical");
        movHorizontal = (int)Input.GetAxis("Horizontal");
        aceleracionactual=aceleracion*movVertical;
        
        rb.AddForce((transform.position-cam.transform.position)*aceleracionactual);
        if(ang.transform.localEulerAngles.y<24 || ang.transform.localEulerAngles.y>336)
        {
            if(movHorizontal!=0)
            {
                aumentogiro+=0.01f;
                anguloreal=ang.transform.localEulerAngles.y;
            }
            
        }
        aceleracionangularactual=aceleracionangular*movHorizontal*aumentogiro;
        rb.AddTorque(Vector3.up*aceleracionangularactual*Time.deltaTime);
        rbw.AddTorque(Vector3.up*aceleracionangularactual*0.21f*Time.deltaTime);
        
        velocidadreal=rb.velocity.magnitude;
        if(anguloreal<=360 && anguloreal>200)
        {
            anguloreal=-360+anguloreal;
        }
        velocidadreal= Mathf.Abs(velocidadreal);
        anguloUI=Mathf.Round(anguloreal * 100f) / 100f;
        anguloreal=Mathf.Round(anguloreal * 10000f) / 10000f;
        velocidadreal=Mathf.Round(velocidadreal * 10000f) / 10000f;
        velocidadUI= Mathf.Round(velocidadreal * 100f) / 100f;
        anguloenvio=anguloreal.ToString();
        if (anguloreal > 0)
            anguloenvio = anguloenvio.Insert(0, "p");
        else if (anguloreal==0)
        {
            anguloenvio= anguloenvio.Insert(0, "z");
        }
        else
            anguloenvio = anguloenvio.Replace("-", "n");

        if (anguloenvio.Contains(","))
            anguloenvio = anguloenvio.Replace(",", "c");
        
        velocidadenvio=velocidadreal.ToString();
        if (velocidadreal==0)
        {
            velocidadenvio= velocidadenvio.Insert(0, "z");
        }
        if (velocidadenvio.Contains(","))
            velocidadenvio = velocidadenvio.Replace(",", "c");
        if (movHorizontal==0)
        {
            
            
            if(anguloreal>0.9f)
            {
                anguloreal-=0.3f;
            }
            else if(anguloreal<-0.9f)
            {
                anguloreal+=0.3f;
            }
            else
            {
                anguloreal=0;
                
            }
            if (aumentogiro>0)
            {
                aumentogiro-=0.05f;
            }
            else
            {
                aumentogiro=0;
            }
            
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                ang.transform.localEulerAngles = new Vector3(rotacionActual.x,0,rotacionActual.z);
                anguloreal=0;
            }
            else
            {
                ang.transform.localEulerAngles = new Vector3(rotacionActual.x,anguloreal,rotacionActual.z);
            }
        }
        
    }          
}