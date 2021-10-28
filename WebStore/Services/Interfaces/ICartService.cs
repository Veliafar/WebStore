using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.ViewModels;

namespace WebStore.Services.Interfaces
{
    public interface ICartService
    {
        void Add(int Id);

        void Decrement(int Id);

        void Remove(int Id);

        void Clear();

        CartViewModel GetViewModel();
    }
}
