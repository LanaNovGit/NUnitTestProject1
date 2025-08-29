pipeline {
  agent any

  environment {
    CI = 'true'
    DOTNET_CLI_TELEMETRY_OPTOUT = '1'
    # Set headless/chrome only if your tests actually use them
    HEADLESS = 'true'
    BROWSER  = 'chrome'
  }

  stages {
    stage('Checkout') {
      steps { checkout scm }
    }

    stage('Restore & Build') {
      steps {
        bat 'dotnet --info'
        // adjust path to your .csproj or .sln (your repo shows the csproj under NUnitTestProject1/)
        bat 'dotnet restore NUnitTestProject1/NUnitTestProject1.csproj'
        bat 'dotnet build NUnitTestProject1/NUnitTestProject1.csproj --configuration Release --no-restore'
      }
    }

    stage('Test') {
      steps {
        // produce a TRX file in a predictable folder
        bat 'dotnet test NUnitTestProject1/NUnitTestProject1.csproj --configuration Release --no-build --logger "trx;LogFileName=test_results.trx" --results-directory TestResults'
      }
    }
  }

  post {
    always {
      // Publish TRX results (MSTest plugin)
      mstest testResultsFile: 'TestResults/**/*.trx', keepLongStdio: true, failOnError: false
      archiveArtifacts artifacts: 'TestResults/**/*.trx', fingerprint: true, onlyIfSuccessful: false
    }
  }
}