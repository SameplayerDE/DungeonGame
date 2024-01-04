namespace UEyeFrame
{
    public class ToggleButton
    {
        public bool IsActive;

        public bool Toogle()
        {
            IsActive = !IsActive;
            return IsActive;
        }
    }
}