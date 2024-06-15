using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Ambacht.Common.Blazor.Components
{
    public class PolymorphicChildComponent<T> : ComponentBase where T : class
    {

        [CascadingParameter()]
        public IPolymorphicParentComponent Parent { get; set; }

        public T Value => Parent?.GetValue<T>();


    }

    public interface IPolymorphicParentComponent
    {
	    T GetValue<T>() where T: class;
    }
}
