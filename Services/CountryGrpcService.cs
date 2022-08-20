
using CountryService.Web.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using static CountryService.Web.Protos.CountryService;

namespace CountryService.Web.Services
{
    public class CountryGrpcService : CountryServiceBase
    {

        private readonly CountryManagementService _countryManagementService;
        public CountryGrpcService(CountryManagementService countryManagementService)
        {
            _countryManagementService = countryManagementService;
        }


        public override async Task GetAll(Empty request, IServerStreamWriter
            <CountryReply> responseStream, ServerCallContext context)
        {

            // Streams all found countries to the client
            var countries = await _countryManagementService.GetAllAsync();
            foreach (var country in countries)
            {
                await responseStream.WriteAsync(country);
            }
            await Task.CompletedTask;

        }
        public override async Task<CountryReply> Get(CountryIdRequest request,
            ServerCallContext context)
        {
            // Send a single country to the client in the gRPC response
            return await _countryManagementService.GetAsync(request);
        }
        public override async Task<Empty> Delete(IAsyncStreamReader<CountryIdRequest> requestStream, ServerCallContext context)
        {
            // Read and store all streamed input messages
            var countryIdRequestList = new List<CountryIdRequest>();
            await foreach (var countryIdRequest in requestStream.
                               ReadAllAsync())
            {
                countryIdRequestList.Add(countryIdRequest);
            }
            // Delete in one shot all streamed countries
            await _countryManagementService.DeleteAsync(countryIdRequestList);
            return new Empty();
        }
        public override async Task<Empty> Update(CountryUpdateRequest request,
            ServerCallContext context)
        {
            // read input message from the gRPC request
            await _countryManagementService.UpdateAsync(request);
            return new Empty();
        }

        public override async Task Create(IAsyncStreamReader<CountryCreationRequest> requestStream, IServerStreamWriter<CountryCreationReply>
            responseStream, ServerCallContext context)
        {
            // Read and store all streamed input messages before performingany action
            var countryCreationRequestList = new List<CountryCreationRequest>();
            await foreach (var countryCreationRequest in requestStream.
                               ReadAllAsync())
            {
                countryCreationRequestList.Add(countryCreationRequest);
            }

            // Call in one shot the countryManagementService that will perform creation operations
            var createdCountries = await _countryManagementService.CreateAsync(
                countryCreationRequestList);
            // Stream all created countries to the client
            foreach (var country in createdCountries)
            {
                await responseStream.WriteAsync(country);
            }


        }


    }
}
