using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Services
{
    public interface IIdGenerator
    {

        string GenerateId(int byteLength);

    }

    public static class IIdGeneratorExtenstions
    {
	    public static string GenerateId(this IIdGenerator generator) => generator.GenerateId(16);
    }
}
