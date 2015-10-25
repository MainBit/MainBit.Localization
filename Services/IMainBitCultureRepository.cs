using MainBit.Localization.Models;
using Orchard;
using Orchard.Caching;
using Orchard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.Localization.Services
{
    public interface IMainBitCultureRepository : IDependency
    {
        List<MainBitCultureRecord> GetList();
        MainBitCultureRecord Get(int id);
        void Create(MainBitCultureRecord domainCultureRecord);
        void Update(MainBitCultureRecord domainCultureRecord);
    }

    public class MainBitCultureRepository : IMainBitCultureRepository
    {
        private readonly IRepository<MainBitCultureRecord> _repository;
        private readonly ISignals _signals;

        public MainBitCultureRepository(IRepository<MainBitCultureRecord> repository,
            ISignals signals)
        {
            _repository = repository;
            _signals = signals;
        }

        public List<MainBitCultureRecord> GetList()
        {
            return _repository.Table.ToList();
        }

        public MainBitCultureRecord Get(int id)
        {
            return _repository.Get(id);
        }

        public void Create(MainBitCultureRecord mainBitCultureRecord)
        {
            _repository.Create(mainBitCultureRecord);
            _signals.Trigger("MainBitCulture.Changed");
        }

        public void Update(MainBitCultureRecord mainBitCultureRecord)
        {
            _repository.Update(mainBitCultureRecord);
            _signals.Trigger("MainBitCulture.Changed");
        }

        public void Delete(MainBitCultureRecord mainBitCultureRecord)
        {
            _repository.Update(mainBitCultureRecord);
            _signals.Trigger("MainBitCulture.Changed");
        }
    }
}