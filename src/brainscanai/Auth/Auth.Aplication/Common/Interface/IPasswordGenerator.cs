using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Common.Interface
{
    public interface IPasswordGenerator
    {
        string Generate(int length = 12);
    }
}
