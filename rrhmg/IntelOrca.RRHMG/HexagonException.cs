using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IntelOrca.RRHMG
{
    [Serializable]
    public class HexagonException : Exception
    {
        public HexagonException()
        {
        }

        public HexagonException(string message)
            : base(message)
        {
        }

        public HexagonException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected HexagonException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
