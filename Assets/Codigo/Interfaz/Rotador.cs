using UnityEngine;

public class Rotador : MonoBehaviour
{
    [Header("Velocidad")]
    [SerializeField] private float ánguloZ;

    private void Update()
    {
        transform.Rotate(0, 0, ánguloZ * Time.deltaTime * 100);
    }

    public void CambiarÁngulo(float nuevoÁnguloZ)
    {
        ánguloZ = nuevoÁnguloZ;
    }
}
