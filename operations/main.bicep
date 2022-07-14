// Parameters
param environmentShortCode string
param regionShortCode string
param location string
param appPlanName string = '${environmentShortCode}-appServicePlan-${regionShortCode}'

@minValue(1)
@maxValue(1)
param aspCapacity int = 1

@allowed([
  'F'
  'B'
])
param aspFamily string = 'F'

@allowed([
  'F1'
  'B1'
  'B2'
])
param aspName string = 'F1'

@allowed([
  'F1'
  'B1'
  'B2'
])
param aspSize string = 'F1'

@allowed([
  'Free'
  'Basic'
])
param aspTier string = 'Free'

param webappName string = '${environmentShortCode}-webApp-${regionShortCode}'

// App Service Plan
resource AppServicePlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: appPlanName
  location: location
  tags: {
    environment: environmentShortCode
    location: regionShortCode
    servoce: 'asp'
  }
  sku: {
    capacity: aspCapacity
    family: aspFamily
    name: aspName
    size: aspSize
    tier: aspTier
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

// Web app
resource webApplication 'Microsoft.Web/sites@2022-03-01' = {
  name: webappName
  location: location
  tags: {
    environment: environmentShortCode
    location: regionShortCode
    servoce: 'web'
  }
  kind: 'app,linux,container'
  properties: {
    serverFarmId: AppServicePlan.id
    siteConfig: {
      linuxFxVersion: 'DOCKER|mcr.microsoft.com/appsvc/staticsite:latest'
      http20Enabled: true
      minTlsVersion: '1.2'
      scmMinTlsVersion: '1.2'
    }
  }
}
