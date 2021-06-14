using electronicComponents.DAL;
using electronicComponents.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace electronicComponents.Service
{
    public interface ISupplierService
    {
        Supplier AddSupplier(Supplier supplier);
        IEnumerable<Supplier> GetSupplierList();
        IEnumerable<Supplier> GetSupplierList(string keyWord);
        List<string> GetSupplierListName(string keyword);
        Supplier GetByID(int ID);
        void UpdateSupplier(Supplier supplier);
        void DeleteSupplier(Supplier supplier);
        void MultiDeleteSupplier(string[] IDs);
        void Block(int ID);
        void Active(int ID);
        void Save();
    }
    public class SupplierService : ISupplierService
    {
        private readonly GenericUnitOfWork context;
        public SupplierService(GenericUnitOfWork repositoryContext)
        {
            this.context = repositoryContext;
        }
        public Supplier AddSupplier(Supplier supplier)
        {
            supplier.lastUpdatedDate = DateTime.Now;
            this.context.GetRepositoryInstance<Supplier>().Add(supplier);
            return supplier;
        }

        public void DeleteSupplier(Supplier supplier)
        {
            supplier.isActive = false;
            this.context.GetRepositoryInstance<Supplier>().Update(supplier);
        }
        public void MultiDeleteSupplier(string[] IDs)
        {
            foreach (var id in IDs)
            {
                Supplier supplier = GetByID(int.Parse(id));
                supplier.isActive = false;
                UpdateSupplier(supplier);
            }
        }

        public Supplier GetByID(int ID)
        {
            return this.context.GetRepositoryInstance<Supplier>().GetFirstorDefault(ID);
        }

        public IEnumerable<Supplier> GetSupplierList()
        {
            IEnumerable<Supplier> listSupplier = this.context.GetRepositoryInstance<Supplier>().GetAllRecords().OrderByDescending(x => x.totalAmount);
            return listSupplier;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void UpdateSupplier(Supplier supplier)
        {
            supplier.lastUpdatedDate = DateTime.Now;
            this.context.GetRepositoryInstance<Supplier>().Update(supplier);
        }

        public IEnumerable<Supplier> GetSupplierList(string keyWord)
        {
            IEnumerable<Supplier> listSupplier = this.context.GetRepositoryInstance<Supplier>().GetAllRecords(x => x.name.Contains(keyWord));
            return listSupplier;
        }

        public void Block(int ID)
        {
            Supplier supplier = context.GetRepositoryInstance<Supplier>().GetFirstorDefault(ID);
            supplier.isActive = false;
            context.GetRepositoryInstance<Supplier>().Update(supplier);
        }

        public void Active(int ID)
        {
            Supplier supplier = context.GetRepositoryInstance<Supplier>().GetFirstorDefault(ID);
            supplier.isActive = true;
            context.GetRepositoryInstance<Supplier>().Update(supplier);
        }

        public List<string> GetSupplierListName(string keyword)
        {
            IEnumerable<Supplier> listSupplierName = this.context.GetRepositoryInstance<Supplier>().GetAllRecords(x => x.name.Contains(keyword) && x.isActive == true);
            List<string> names = new List<string>();
            foreach (var item in listSupplierName)
            {
                names.Add(item.name);
            }
            return names;
        }
    }
}