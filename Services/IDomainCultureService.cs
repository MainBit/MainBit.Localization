using MainBit.Localization.Models;
using Orchard;
using Orchard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MainBit.Localization.Services
{
    public interface IDomainCultureService : IDependency
    {
        List<DomainCultureRecord> GetCultures();
        DomainCultureRecord Get(int id);
        void Create(DomainCultureRecord domainCultureRecord);
        void Update(DomainCultureRecord domainCultureRecord);
    }

    public class DomainCultureService : IDomainCultureService
    {
        private readonly IRepository<DomainCultureRecord> _domainCultureRepository;

        public DomainCultureService(IRepository<DomainCultureRecord> domainCultureRepository)
        {
            _domainCultureRepository = domainCultureRepository;
        }
        public List<DomainCultureRecord> GetCultures()
        {
            return _domainCultureRepository.Table.ToList();
        }

        public DomainCultureRecord Get(int id)
        {
            return _domainCultureRepository.Get(id);
        }

        public void Create(DomainCultureRecord domainCultureRecord)
        {
            _domainCultureRepository.Create(domainCultureRecord);
        }

        public void Update(DomainCultureRecord domainCultureRecord)
        {
            _domainCultureRepository.Update(domainCultureRecord);
        }
    }
}