using ModeloValidador.Abstracciones;

namespace SixCharacters3Letters3Numbers;

public class SixCharacters3Letters3NumbersModelValidator : IModeloValidador
{
    public bool EsValido(Modelo modelo)
    {
        return modelo.Value.Length == 6 &&
               modelo.Value.Substring(0, 3).All(char.IsLetter) &&
               modelo.Value.Substring(3, 3).All(char.IsDigit);
    }
}
