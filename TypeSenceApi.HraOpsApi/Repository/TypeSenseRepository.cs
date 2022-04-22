using TypeSenceApi.HraOpsApi.DapperUnitOfWork;
using Dapper;
using Typesense;
using Typesense.Setup;
using TypeSenceApi.HraOpsApi.Model;
using Microsoft.Extensions.Options;

namespace TypeSenceApi.HraOpsApi.Repository
{
    public class TypeSenseRepository : ITypeSenseRepository
    {

        ITypesenseClient? _typesenseClient;
        IOptions<AppSettings> _iOption;
        IUnitOfWork _iUnitOfWork;

        public TypeSenseRepository(IOptions<AppSettings> iOption, IUnitOfWork iUnitOfWork)
        {
            _iOption = iOption;
            _iUnitOfWork = iUnitOfWork;
            var _serviceProvider = new ServiceCollection().AddTypesenseClient(config =>
            {
                config.ApiKey = _iOption.Value.TypeSenseApiKey;
                config.Nodes = new List<Node>
                         {
                            new Node
                            {
                                Host = _iOption.Value.TypeSenseHost,
                                Port = _iOption.Value.TypeSensePort,
                                Protocol = _iOption.Value.TypeSenseProtocol
                            }
                         };
            }).BuildServiceProvider();
            _typesenseClient = _serviceProvider.GetService<ITypesenseClient>();
        }




        public async Task<Webresponse<jobseeker_resume>> UpdateInsert(long rid)
        {
            Webresponse<jobseeker_resume> webresponse = new Webresponse<jobseeker_resume>();
            try
            {

                string sql = getQuerySingleRecordUpdate(rid);
                jobseeker_resume hC_ResumeBanks = await _iUnitOfWork.Connection.QueryFirstOrDefaultAsync<jobseeker_resume>(sql);
                if (hC_ResumeBanks != null)
                {
                    webresponse.data = hC_ResumeBanks;
                    var query = new SearchParameters
                    {

                        Text = hC_ResumeBanks.jobseekerid,
                        QueryBy = "jobseekerid"
                    };
                    var searchResult = await _typesenseClient.Search<jobseeker_resume>(_iOption.Value.TypeSenseIndexName, query);
                    if (searchResult.Hits.Any(z => z.Document.rid == rid))
                    {
                        await _typesenseClient.UpdateDocument<jobseeker_resume>(_iOption.Value.TypeSenseIndexName, searchResult.Hits.Where(z => z.Document.rid == rid).FirstOrDefault()?.Document.id, hC_ResumeBanks);
                        webresponse.message = "update document index";
                    }
                    else
                    {
                        var createDocumentResult = await _typesenseClient.CreateDocument<jobseeker_resume>(_iOption.Value.TypeSenseIndexName, hC_ResumeBanks);
                        webresponse.message = "New document index";
                    }

                    webresponse.status = APIStatus.success;
                }
                else
                {
                    webresponse.message = $"JobSeeker not found for this Rid {rid}";
                    webresponse.status = APIStatus.Notfound;
                }


            }
            catch (Exception ex)
            {
                webresponse.message = ex.Message;
                webresponse.status = APIStatus.error;
            }
            return webresponse;
        }

        public async Task<Webresponse<jobseeker_resume>> Get(long rid)
        {
            Webresponse<jobseeker_resume> webresponse = new Webresponse<jobseeker_resume>();
            try
            {

                string sql = getQuerySingleRecordUpdate(rid);
                jobseeker_resume hC_ResumeBanks = await _iUnitOfWork.Connection.QueryFirstOrDefaultAsync<jobseeker_resume>(sql);
                if (hC_ResumeBanks != null)
                {

                    var query = new SearchParameters
                    {

                        Text = hC_ResumeBanks.jobseekerid,
                        QueryBy = "jobseekerid"
                    };
                    var searchResult = await _typesenseClient.Search<jobseeker_resume>(_iOption.Value.TypeSenseIndexName, query);
                    if (searchResult.Hits.Any(z => z.Document.rid == rid))
                    {

                        
                        webresponse.data = searchResult.Hits.Where(z => z.Document.rid == rid).FirstOrDefault().Document;
                        webresponse.status = APIStatus.success;
                    }
                    else
                    {
                        webresponse.status = APIStatus.Notfound;
                    }
                }
                else
                {
                    webresponse.message = $"JobSeeker not found for this Rid {rid}";
                    webresponse.status = APIStatus.Notfound;
                }

            }
            catch (Exception ex)
            {
                webresponse.message = ex.Message;
                webresponse.status = APIStatus.error;
            }
            return webresponse;
        }



        private string getQuerySingleRecordUpdate(long rid)
        {
            string strQry = "select ";
           
            return strQry;
        }


    }
}
