namespace EvoMotion2D.Modules
{
    public class SignedParameter : Parameter
    {
        public SignedParameter() : base()
        {
            Value = Util.SignedRange(InitialValueRange);
        }
    }
}