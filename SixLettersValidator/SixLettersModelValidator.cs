using ModeloValidador.Abstracciones;

namespace SixLettersValidator;

public class SixLettersModelValidator : IModeloValidador
{
    public bool EsValido(Modelo modelo)
    {
        return modelo.Value.Length == 6;
    }
}
