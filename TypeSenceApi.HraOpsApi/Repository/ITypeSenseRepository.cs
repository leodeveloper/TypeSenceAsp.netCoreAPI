
using TypeSenceApi.HraOpsApi.Model;

namespace TypeSenceApi.HraOpsApi.Repository
{
    public interface ITypeSenseRepository
    {
        Task<Webresponse<jobseeker_resume>> UpdateInsert(long rid);
        Task<Webresponse<jobseeker_resume>> Get(long rid);
    }
}