pipeline {
  agent any
  environment {
    CI = 'true'
    DOTNET_CLI_TELEMETRY_OPTOUT = '1'
  }
  stages {
    stage('Checkout') { steps { checkout scm } }

    stage('Restore & Build') {
      steps {
        bat 'dotnet --info'
        bat 'dir /b *.sln *.csproj'
        bat 'dotnet restore NUnitTestProject1.csproj'
        bat 'dotnet build NUnitTestProject1.csproj --configuration Release --no-restore'
      }
    }

    stage('Test') {
      steps {
        bat 'dotnet test NUnitTestProject1.csproj --configuration Release --no-build --logger "trx;LogFileName=test_results.trx" --results-directory TestResults'
      }
    }
  }
  post {
    always {
      mstest testResultsFile: 'TestResults/**/*.trx', keepLongStdio: true, failOnError: false
      archiveArtifacts artifacts: 'TestResults/**/*.trx', fingerprint: true, onlyIfSuccessful: false
    }
  }
}