.PHONY: build test coverage clean restore publish

all: restore build test

restore:
	dotnet restore

build:
	dotnet build --no-restore

test:
	dotnet test --no-build --no-restore \
		--collect:"XPlat Code Coverage" \
	    --results-directory ./TestResults

	reportgenerator \
	    -reports:./TestResults/**/coverage.cobertura.xml \
	    -targetdir:./CoverageReport \
	    -reporttypes:Html;lcov
	
publish:
	dotnet publish src/MyApp/MyApp.csproj -c Release -o out

clean:
	dotnet clean