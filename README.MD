grpcurl -d '{\"Id\": 1}' localhost:7151 gRPCDemo.v1.CountryService/Get

grpcurl -d '{\"Id\": 1}{\"Id\": 2}' localhost:7151 gRPCDemo.v1.CountryService/Delete


grpcui localhost:7151