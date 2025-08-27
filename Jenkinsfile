pipeline {
  agent any
  environment {
    CI = "true"
    HEADLESS = "true"
    BROWSER = "chrome"
  }
  stages {
    stage('Checkout') {
      steps { checkout scm }
    }
    stage('Build & Test') {
      steps {
        sh 'dotnet test NUnitTestProject1/NUnitTestProject1.csproj --configuration Release --logger "trx;LogFileName=test_results.trx"'
      }
    }
  }
  post {
    always {
      mstest testResultsFile: '**/test_results.trx', keepLongStdio: true, failOnError: false
    }
  }
}
