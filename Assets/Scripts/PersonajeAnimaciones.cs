using UnityEngine;

public class PersonajeAnimaciones : MonoBehaviour
{

    private Animator _animator;
    private PersonajeMovimiento _personajeMovimiento;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _personajeMovimiento = GetComponent<PersonajeMovimiento>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        _animator.SetFloat("X", _personajeMovimiento.DireccionMovimiento.x);
        _animator.SetFloat("Y", _personajeMovimiento.DireccionMovimiento.y);
    }
}


