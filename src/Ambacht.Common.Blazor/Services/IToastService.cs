using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambacht.Common.Blazor.Services
{
    public interface IToastService : INotifyPropertyChanged
    {

        void Success(string title, string message);

        void Info(string title, string message);

        void Warning(string title, string message);

        void Error(string title, string message);

        ToastMessage Message { get; set; }

	}

    public class DummyToastService : IToastService
    {
	    public event PropertyChangedEventHandler PropertyChanged;
	    public void Success(string title, string message)
	    {
	    }

	    public void Info(string title, string message)
	    {
	    }

	    public void Warning(string title, string message)
	    {
	    }

	    public void Error(string title, string message)
	    {
	    }

	    public ToastMessage Message { get; set; }
    }
}
