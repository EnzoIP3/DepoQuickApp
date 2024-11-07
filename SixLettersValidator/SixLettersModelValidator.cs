using ModeloValidador.Abstracciones;

namespace SixLettersModelValidator;

public class SixLettersModelValidator : IModeloValidador
{
    public bool EsValido(Modelo modelo)
    {
        return modelo.Value.Length == 6;
    }
}
