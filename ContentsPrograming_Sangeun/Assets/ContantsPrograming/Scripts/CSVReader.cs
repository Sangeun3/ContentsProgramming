using UnityEngine;
using System.IO;     // File 클래스
using System.Text;   // Encoding 클래스

public class CSVReader : MonoBehaviour
{
    void Start()
    {
        string dirty = "   사과   ";
        string clean = dirty.Trim();

        Debug.Log(dirty);
        Debug.Log(clean);
    }
}