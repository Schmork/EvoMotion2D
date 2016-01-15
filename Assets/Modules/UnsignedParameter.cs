namespace EvoMotion2D.Modules
{
    public class UnsignedParameter : Parameter
    {
        public UnsignedParameter() : base()
        {          
			Value = Util.UnsignedRange(InitialValueRange);
        }
    }
}
