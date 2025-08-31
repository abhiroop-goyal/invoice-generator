.PHONY: build test coverage clean restore publish

all: restore build test

restore:
	dotnet restore

build:
	dotnet build --no-restore

test:
	dotnet test --no-build --no-restore \
		/p:CollectCoverage=true \
		/p:CoverletOutput=TestResults/coverage.info \
		/p:CoverletOutputFormat=lcov
	
publish:
	dotnet publish src/MyApp/MyApp.csproj -c Release -o out

clean:
	dotnet clean