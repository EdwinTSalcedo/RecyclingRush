using System.Collections;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
public class Create_documents : MonoBehaviour
{
    private void Awake()
    {
        string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        // Define la ruta de las carpetas
        string recyclingRushPath = Path.Combine(documentsPath, "!Recycling Rush");
        
        // Verifica si las carpetas existen, si no, las crea
        if (!Directory.Exists(recyclingRushPath))
        {
            Directory.CreateDirectory(recyclingRushPath);
        }
        
    }
    
}
