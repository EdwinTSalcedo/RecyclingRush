using UnityEngine;
using System.Net;
using System.Text;
public class Movimiento : MonoBehaviour
{
    public GameObject cam;
    public GameObject ang;
    public float aceleracion=500f;
    public float aceleracionangular=5f;
    public float aceleracionactual=0f;
    public float aceleracionangularactual=0f;
    public Rigidbody rb;
    public Rigidbody rbw;
    public float movHorizontal=0;
    public float movVertical=0;
    public float anguloreal=0;
    public float velocidadreal=0;
    public int region;
    private int valorx;
    private int valorz;

    private void Start()
    {
        rb=GetComponent<Rigidbody>();   
        rbw=ang.GetComponent<Rigidbody>();
        region=Random.Range(0,10);
        switch (region)
        {
            case 0:
                valorx=Random.Range(-65,-8);
                valorz=Random.Range(-170,-110);
                
                break;
            case 1:
                valorx=Random.Range(122,204);
                valorz=Random.Range(-290,-222);
                break;
            case 2:
                valorx=Random.Range(105,180);
                valorz=Random.Range(-172,-105);
                break;
            case 3:
                valorx=Random.Range(-122,-70);
                valorz=Random.Range(-133,-87);
                break;
        }
        transform.position = new Vector3(valorx, 1f, valorz);
    }
    private void FixedUpdate()
    {
        movVertical = Input.GetAxis("Vertical");
        movHorizontal = Input.GetAxis("Horizontal");
        aceleracionactual=aceleracion*movVertical;
        aceleracionangularactual=aceleracionangular*movHorizontal;
        rb.AddForce((transform.position-cam.transform.position)*aceleracionactual);
        if(ang.transform.localEulerAngles.y<118 || ang.transform.localEulerAngles.y>242)
        {
            rb.AddTorque(Vector3.up*aceleracionangularactual*Time.deltaTime);
            rbw.AddTorque(Vector3.up*aceleracionangularactual*0.21f*Time.deltaTime);
        }
        anguloreal=ang.transform.localEulerAngles.y;
        velocidadreal=rb.velocity.z;
        if(anguloreal<360 && anguloreal>200)
        {
            anguloreal=-360+anguloreal;
        }
        anguloreal= Mathf.Round(anguloreal * 100f) / 100f;
        velocidadreal= Mathf.Round(velocidadreal * 100f) / 100f;
        velocidadreal= Mathf.Abs(velocidadreal);
        if (movHorizontal==0)
        {
            ang.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f); 
        }
    }          
}