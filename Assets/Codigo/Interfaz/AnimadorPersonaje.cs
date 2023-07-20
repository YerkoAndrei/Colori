using UnityEngine;
using static Constantes;

public class AnimadorPersonaje : MonoBehaviour
{
    [Header("Personaje")]
    [SerializeField] private Animator animador;
    [SerializeField] private GameObject caraNormal;
    [SerializeField] private GameObject caraFeliz;
    [SerializeField] private GameObject caraEnojada;
    [SerializeField] private GameObject caraSorprendida;

    [Header("Animaciones")]
    [SerializeField] private AnimationClip animaci�nQuietud;
    [SerializeField] private AnimationClip animaci�nDerecha;
    [SerializeField] private AnimationClip animaci�nIzquierda;
    [SerializeField] private AnimationClip animaci�nFeliz;
    [SerializeField] private AnimationClip animaci�nEnojada;
    [SerializeField] private AnimationClip animaci�nSorprendida;

    private void Start()
    {
        // Reemplazo de animaciones en controlador estandar
        var reemplazoAnimador = new AnimatorOverrideController();
        reemplazoAnimador.runtimeAnimatorController = ObtenerReemplazoControlador(animador);

        // Animaciones estandar
        reemplazoAnimador["Quietud"] = animaci�nQuietud;
        reemplazoAnimador["Derecha"] = animaci�nDerecha;
        reemplazoAnimador["Izquierda"] = animaci�nIzquierda;
        reemplazoAnimador["Feliz"] = animaci�nFeliz;
        reemplazoAnimador["Enojada"] = animaci�nEnojada;

        animador.runtimeAnimatorController = reemplazoAnimador;
        AnimarPersonaje(Animaciones.normal);
    }

    public void AnimarPersonaje(Animaciones animacion)
    {
        ApagarCaras();

        switch (animacion)
        {
            case Animaciones.normal:
                caraNormal.SetActive(true);
                animador.SetTrigger("Quietud");
                break;
            case Animaciones.entrarDerecha:
                caraFeliz.SetActive(true);
                animador.SetTrigger("Derecha");
                break;
            case Animaciones.entrarIzquierda:
                caraFeliz.SetActive(true);
                animador.SetTrigger("Izquierda");
                break;
            case Animaciones.salirDerecha:
                caraEnojada.SetActive(true);
                animador.SetTrigger("Derecha");
                break;
            case Animaciones.salirIzquierda:
                caraEnojada.SetActive(true);
                animador.SetTrigger("Izquierda");
                break;
            case Animaciones.feliz:
                caraFeliz.SetActive(true);
                animador.SetTrigger("Feliz");
                break;
            case Animaciones.enojada:
                caraEnojada.SetActive(true);
                animador.SetTrigger("Enojada");
                break;
            case Animaciones.sorprendida:
                caraSorprendida.SetActive(true);
                animador.SetTrigger("Sorprendida");
                break;
        }
    }

    private void ApagarCaras()
    {
        caraNormal.SetActive(false);
        caraFeliz.SetActive(false);
        caraEnojada.SetActive(false);
        caraSorprendida.SetActive(false);
    }

    // Unity doc
    public RuntimeAnimatorController ObtenerReemplazoControlador(Animator animator)
    {
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;
        AnimatorOverrideController overrideController = controller as AnimatorOverrideController;
        while (overrideController != null)
        {
            controller = overrideController.runtimeAnimatorController;
            overrideController = controller as AnimatorOverrideController;
        }
        return controller;
    }
}
