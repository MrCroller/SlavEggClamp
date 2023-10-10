namespace SEC.Character.Controller
{
    public interface IMovable
    {
        void Move(float moven, bool crouch, bool jump);
        void Hand();
        void Kicked();
        void Bump(float forse);
    }
}