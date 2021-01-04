using System;

namespace NewCoreClassLibrary
{
    public class Utility
    {

        public bool IsAdult(int age)
        {
            if (age >= 18)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
