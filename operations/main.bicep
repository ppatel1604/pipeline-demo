// Parameters
param environmentShortCode string
param regionShortCode string = 'wus'
param location string = 'west us'
param appPlanName string = '${environmentShortCode}-appServicePlan-${regionShortCode}'

@minValue(1)
@maxValue(1)
param aspCapacity int = 1

@allowed([
  'F'
  'S'
])
param aspFamily string = 'F'

@allowed([
  'F1'
  'S1'
])
param aspName string = 'F1'

@allowed([
  'F1'
  'S1'
])
param aspSize string = 'F1'

@allowed([
  'Free'
  'Standard'
])
param aspTier string = 'Free'


// App Service Plan
resource AppServicePlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: appPlanName
  location: location
  tags: {
    environment: environmentShortCode
    location: regionShortCode
  }
  sku: {
    capacity: aspCapacity
    family: aspFamily
    name: aspName
    size: aspSize
    tier: aspTier
  }
  kind: 'linux'
}
