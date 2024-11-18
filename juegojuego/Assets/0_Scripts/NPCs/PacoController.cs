using PixelCrushers.DialogueSystem;
using UnityEngine;

public class PacoController : MonoBehaviour
{
    private Animator animator;

    private void OnEnable()
    {
        Lua.RegisterFunction(nameof(PlayIdlePaco), this, SymbolExtensions.GetMethodInfo(() => PlayIdlePaco()));
        Lua.RegisterFunction(nameof(PlayEnfadaoPaco), this, SymbolExtensions.GetMethodInfo(() => PlayEnfadaoPaco()));
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void PlayEnfadaoPaco()
    {
        animator.Play("Enfadao");
    }

    private void PlayIdlePaco()
    {
        animator.Play("Idle");
    }

    private void OnDisable()
    {
        Lua.UnregisterFunction(nameof(PlayIdlePaco));
        Lua.UnregisterFunction(nameof(PlayEnfadaoPaco));
    }
}
