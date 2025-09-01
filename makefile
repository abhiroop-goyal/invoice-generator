.PHONY: build test coverage clean restore publish

all: restore build test

setup:
	dotnet tool restore

restore:
	dotnet restore

build:
	dotnet build --no-restore

test:
	dotnet test --no-build --no-restore \
		--collect:"XPlat Code Coverage" \
	    --results-directory ./TestResults

	dotnet tool run reportgenerator \
	    -reports:./TestResults/**/coverage.cobertura.xml \
	    -targetdir:./out/coveragereport \
	    -reporttypes:"Html;lcov"
	
publish:
	dotnet publish src/MyApp/MyApp.csproj -c Release -o out

clean:
	dotnet clean