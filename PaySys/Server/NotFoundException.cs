using System;

namespace PaySys.Server
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException() : base("Object not found")
        {
        }
    }
}
