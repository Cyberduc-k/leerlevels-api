@description('Generated from /subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.Storage/storageAccounts/leerlevelsstorage')
resource leerlevelsstorage 'Microsoft.Storage/storageAccounts@2022-05-01' = {
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'Storage'
  name: 'leerlevelsstorage'
  location: 'westeurope'
  tags: {
  }
  properties: {
    minimumTlsVersion: 'TLS1_2'
    allowBlobPublicAccess: true
    networkAcls: {
      bypass: 'AzureServices'
      virtualNetworkRules: []
      ipRules: []
      defaultAction: 'Allow'
    }
    supportsHttpsTrafficOnly: true
    encryption: {
      services: {
        file: {
          keyType: 'Account'
          enabled: true
        }
        blob: {
          keyType: 'Account'
          enabled: true
        }
      }
      keySource: 'Microsoft.Storage'
    }
  }
}

@description('Generated from /subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.Storage/storageAccounts/leerlevelsstorage/blobServices/default')
resource defaultBlobStorage 'Microsoft.Storage/storageAccounts/blobServices@2022-05-01' = {
  name: 'leerlevelsstorage/default'
  properties: {
    cors: {
      corsRules: []
    }
    deleteRetentionPolicy: {
      allowPermanentDelete: false
      enabled: false
    }
  }
}

@description('Generated from /subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.Storage/storageAccounts/leerlevelsstorage/fileServices/default')
resource defaultFileService 'Microsoft.Storage/storageAccounts/fileServices@2022-05-01' = {
  name: 'leerlevelsstorage/default'
  properties: {
    protocolSettings: {
      smb: {
      }
    }
    cors: {
      corsRules: []
    }
    shareDeleteRetentionPolicy: {
      enabled: true
      days: 7
    }
  }
}

@description('Generated from /subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.Storage/storageAccounts/leerlevelsstorage/queueServices/default')
resource defaultQueue 'Microsoft.Storage/storageAccounts/queueServices@2022-05-01' = {
  name: 'leerlevelsstorage/default'
  properties: {
    logging: {
      version: '1.0'
      delete: false
      read: false
      write: false
      retentionPolicy: {
        enabled: false
      }
    }
    cors: {
      corsRules: []
    }
  }
}

@description('Generated from /subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.Web/serverFarms/ASP-LeerLevels-859a')
resource ASPLeerLevelsa 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'ASP-LeerLevels-859a'
  kind: 'functionapp'
  location: 'West Europe'
  tags: {
  }
  properties: {
    serverFarmId: 45839
    name: 'ASP-LeerLevels-859a'
    workerSize: 'Default'
    workerSizeId: 0
    currentWorkerSize: 'Default'
    currentWorkerSizeId: 0
    currentNumberOfWorkers: 0
    webSpace: 'LeerLevels-WestEuropewebspace'
    planName: 'VirtualDedicatedPlan'
    computeMode: 'Dynamic'
    perSiteScaling: false
    elasticScaleEnabled: false
    maximumElasticWorkerCount: 0
    isSpot: false
    tags: {
    }
    kind: 'functionapp'
    reserved: false
    isXenon: false
    hyperV: false
    mdmId: 'waws-prod-am2-453_45839'
    targetWorkerCount: 0
    targetWorkerSizeId: 0
    zoneRedundant: false
  }
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
    size: 'Y1'
    family: 'Y'
    capacity: 0
  }
}

@description('Generated from /subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.Web/sites/leerlevels')
resource leerlevels 'Microsoft.Web/sites@2022-03-01' = {
  name: 'leerlevels'
  kind: 'functionapp'
  location: 'West Europe'
  tags: {
    'hidden-link: /app-insights-resource-id': '/subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/microsoft.insights/components/leerlevels'
    'hidden-link: /app-insights-instrumentation-key': '334042ff-ea47-474b-95ca-10b9efa87f81'
    'hidden-link: /app-insights-conn-string': 'InstrumentationKey=334042ff-ea47-474b-95ca-10b9efa87f81;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/'
  }
  properties: {
    name: 'leerlevels'
    webSpace: 'LeerLevels-WestEuropewebspace'
    selfLink: 'https://waws-prod-am2-453.api.azurewebsites.windows.net:454/subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/webspaces/LeerLevels-WestEuropewebspace/sites/leerlevels'
    enabled: true
    adminEnabled: true
    siteProperties: {
      metadata: null
      properties: [
        {
          name: 'LinuxFxVersion'
          value: ''
        }
        {
          name: 'WindowsFxVersion'
          value: null
        }
      ]
      appSettings: null
    }
    csrs: []
    hostNameSslStates: [
      {
        name: 'leerlevels.azurewebsites.net'
        sslState: 'Disabled'
        ipBasedSslState: 'NotConfigured'
        hostType: 'Standard'
      }
      {
        name: 'leerlevels.scm.azurewebsites.net'
        sslState: 'Disabled'
        ipBasedSslState: 'NotConfigured'
        hostType: 'Repository'
      }
    ]
    serverFarmId: '/subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.Web/serverfarms/ASP-LeerLevels-859a'
    reserved: false
    isXenon: false
    hyperV: false
    storageRecoveryDefaultState: 'Running'
    contentAvailabilityState: 'Normal'
    runtimeAvailabilityState: 'Normal'
    vnetRouteAllEnabled: false
    vnetImagePullEnabled: false
    vnetContentShareEnabled: false
    siteConfig: {
      numberOfWorkers: 1
      linuxFxVersion: ''
      acrUseManagedIdentityCreds: false
      alwaysOn: false
      http20Enabled: false
      functionAppScaleLimit: 200
      minimumElasticInstanceCount: 0
    }
    deploymentId: 'leerlevels'
    sku: 'Dynamic'
    scmSiteAlsoStopped: false
    clientAffinityEnabled: false
    clientCertEnabled: false
    clientCertMode: 'Required'
    hostNamesDisabled: false
    customDomainVerificationId: 'E6AD022DA781E86BD34C6B5AA096F78ED94923648789AE4196A5622B106F0801'
    kind: 'functionapp'
    inboundIpAddress: '20.50.2.63'
    possibleInboundIpAddresses: '20.50.2.63'
    ftpUsername: 'leerlevels\\$leerlevels'
    ftpsHostName: 'ftps://waws-prod-am2-453.ftp.azurewebsites.windows.net/site/wwwroot'
    containerSize: 1536
    dailyMemoryTimeQuota: 0
    siteDisabledReason: 0
    homeStamp: 'waws-prod-am2-453'
    tags: {
      'hidden-link: /app-insights-resource-id': '/subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/microsoft.insights/components/leerlevels'
      'hidden-link: /app-insights-instrumentation-key': '334042ff-ea47-474b-95ca-10b9efa87f81'
      'hidden-link: /app-insights-conn-string': 'InstrumentationKey=334042ff-ea47-474b-95ca-10b9efa87f81;IngestionEndpoint=https://westeurope-5.in.applicationinsights.azure.com/;LiveEndpoint=https://westeurope.livediagnostics.monitor.azure.com/'
    }
    httpsOnly: true
    redundancyMode: 'None'
    privateEndpointConnections: []
    eligibleLogCategories: 'FunctionAppLogs'
    storageAccountRequired: false
    keyVaultReferenceIdentity: 'SystemAssigned'
  }
  identity: {
    type: 'SystemAssigned'
  }
}

@description('Generated from /subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.Web/serverFarms/ASP-LeerLevels-b96d')
resource ASPLeerLevelsbd 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: 'ASP-LeerLevels-b96d'
  kind: 'functionapp'
  location: 'West Europe'
  tags: {
  }
  properties: {
    serverFarmId: 46861
    name: 'ASP-LeerLevels-b96d'
    workerSize: 'Default'
    workerSizeId: 0
    currentWorkerSize: 'Default'
    currentWorkerSizeId: 0
    currentNumberOfWorkers: 0
    webSpace: 'LeerLevels-WestEuropewebspace'
    planName: 'VirtualDedicatedPlan'
    computeMode: 'Dynamic'
    perSiteScaling: false
    elasticScaleEnabled: false
    maximumElasticWorkerCount: 1
    isSpot: false
    tags: {
    }
    kind: 'functionapp'
    reserved: false
    isXenon: false
    hyperV: false
    mdmId: 'waws-prod-am2-453_46861'
    targetWorkerCount: 0
    targetWorkerSizeId: 0
    zoneRedundant: false
  }
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
    size: 'Y1'
    family: 'Y'
    capacity: 0
  }
}

@description('Generated from /subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.Web/sites/leerlevels-notifications')
resource leerlevelsnotifications 'Microsoft.Web/sites@2022-03-01' = {
  name: 'leerlevels-notifications'
  kind: 'functionapp'
  location: 'West Europe'
  tags: {
    'hidden-link: /app-insights-resource-id': '/subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.Insights/components/leerlevels-notifications'
  }
  properties: {
    name: 'leerlevels-notifications'
    webSpace: 'LeerLevels-WestEuropewebspace'
    selfLink: 'https://waws-prod-am2-453.api.azurewebsites.windows.net:454/subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/webspaces/LeerLevels-WestEuropewebspace/sites/leerlevels-notifications'
    enabled: true
    adminEnabled: true
    siteProperties: {
      metadata: null
      properties: [
        {
          name: 'LinuxFxVersion'
          value: ''
        }
        {
          name: 'WindowsFxVersion'
          value: null
        }
      ]
      appSettings: null
    }
    csrs: []
    hostNameSslStates: [
      {
        name: 'leerlevels-notifications.azurewebsites.net'
        sslState: 'Disabled'
        ipBasedSslState: 'NotConfigured'
        hostType: 'Standard'
      }
      {
        name: 'leerlevels-notifications.scm.azurewebsites.net'
        sslState: 'Disabled'
        ipBasedSslState: 'NotConfigured'
        hostType: 'Repository'
      }
    ]
    serverFarmId: '/subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.Web/serverfarms/ASP-LeerLevels-b96d'
    reserved: false
    isXenon: false
    hyperV: false
    storageRecoveryDefaultState: 'Running'
    contentAvailabilityState: 'Normal'
    runtimeAvailabilityState: 'Normal'
    vnetRouteAllEnabled: false
    vnetImagePullEnabled: false
    vnetContentShareEnabled: false
    siteConfig: {
      numberOfWorkers: 1
      linuxFxVersion: ''
      acrUseManagedIdentityCreds: false
      alwaysOn: false
      http20Enabled: false
      functionAppScaleLimit: 200
      minimumElasticInstanceCount: 0
    }
    deploymentId: 'leerlevels-notifications'
    sku: 'Dynamic'
    scmSiteAlsoStopped: false
    clientAffinityEnabled: false
    clientCertEnabled: false
    clientCertMode: 'Required'
    hostNamesDisabled: false
    customDomainVerificationId: 'E6AD022DA781E86BD34C6B5AA096F78ED94923648789AE4196A5622B106F0801'
    kind: 'functionapp'
    inboundIpAddress: '20.50.2.63'
    possibleInboundIpAddresses: '20.50.2.63'
    ftpUsername: 'leerlevels-notifications\\$leerlevels-notifications'
    ftpsHostName: 'ftps://waws-prod-am2-453.ftp.azurewebsites.windows.net/site/wwwroot'
    containerSize: 1536
    dailyMemoryTimeQuota: 0
    siteDisabledReason: 0
    homeStamp: 'waws-prod-am2-453'
    tags: {
      'hidden-link: /app-insights-resource-id': '/subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.Insights/components/leerlevels-notifications'
    }
    httpsOnly: true
    redundancyMode: 'None'
    privateEndpointConnections: []
    eligibleLogCategories: 'FunctionAppLogs'
    storageAccountRequired: false
    keyVaultReferenceIdentity: 'SystemAssigned'
  }
}

@description('Generated from /subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.KeyVault/vaults/LeerLevelsKeys')
resource LeerLevelsKeys 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: 'LeerLevelsKeys'
  location: 'westeurope'
  tags: {
  }
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: '45abd59b-c088-4d47-a733-4cc559021d43'
    accessPolicies: [
      {
        tenantId: '45abd59b-c088-4d47-a733-4cc559021d43'
        objectId: 'fa87f0b6-5c26-48bf-8086-395b2e5d374a'
        permissions: {
          keys: [
            'Get'
            'List'
            'Update'
            'Create'
            'Import'
            'Delete'
            'Recover'
            'Backup'
            'Restore'
            'GetRotationPolicy'
            'SetRotationPolicy'
            'Rotate'
          ]
          secrets: [
            'Get'
            'List'
            'Set'
            'Delete'
            'Recover'
            'Backup'
            'Restore'
          ]
          certificates: [
            'Get'
            'List'
            'Update'
            'Create'
            'Import'
            'Delete'
            'Recover'
            'Backup'
            'Restore'
            'ManageContacts'
            'ManageIssuers'
            'GetIssuers'
            'ListIssuers'
            'SetIssuers'
            'DeleteIssuers'
          ]
        }
      }
      {
        tenantId: '45abd59b-c088-4d47-a733-4cc559021d43'
        objectId: '173841e6-ab28-4b64-ac59-51a2adc77a40'
        permissions: {
          certificates: []
          keys: []
          secrets: [
            'Get'
            'List'
          ]
        }
      }
    ]
    enabledForDeployment: false
    enabledForDiskEncryption: false
    enabledForTemplateDeployment: false
    enableSoftDelete: true
    softDeleteRetentionInDays: 90
    enableRbacAuthorization: false
    vaultUri: 'https://leerlevelskeys.vault.azure.net/'
    provisioningState: 'Succeeded'
    publicNetworkAccess: 'Enabled'
  }
}

@description('Generated from /subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.NotificationHubs/namespaces/LeerLevelsNotifications')
resource LeerLevelsNotifications 'Microsoft.NotificationHubs/namespaces@2017-04-01' = {
  sku: {
    name: 'Free'
  }
  properties: {
    provisioningState: 'Succeeded'
    status: 'Active'
    createdAt: '2022-10-19T15:57:07.8200000Z'
    updatedAt: '2022-10-19T15:57:07.8300000Z'
    serviceBusEndpoint: 'https://LeerLevelsNotifications.servicebus.windows.net:443/'
    enabled: true
    critical: false
    zoneRedundant: false
  }
  name: 'LeerLevelsNotifications'
  location: 'West Europe'
  tags: {
  }
}

@description('Generated from /subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.NotificationHubs/namespaces/LeerLevelsNotifications/NotificationHubs/LeerLevelsNotificationHub')
resource LeerLevelsNotificationHub 'Microsoft.NotificationHubs/namespaces/notificationHubs@2017-04-01' = {
  properties: {
    registrationTtl: '10675199.02:48:05.4775807'
    authorizationRules: []
    dailyMaxActiveDevices: 0
  }
  name: 'LeerLevelsNotifications/LeerLevelsNotificationHub'
  location: 'West Europe'
}

@description('Generated from /subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.Sql/servers/leerlevels')
resource leerlevelsSqlServer 'Microsoft.Sql/servers@2022-05-01-preview' = {
  properties: {
    administratorLogin: 'LEERLEVELS_ADMIN'
    version: '12.0'
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Disabled'
    administrators: {
      administratorType: 'ActiveDirectory'
      principalType: 'Application'
      login: '648680-registration'
      sid: '880460a0-fd9c-42d7-88c7-96db835fd863'
      tenantId: '45abd59b-c088-4d47-a733-4cc559021d43'
      azureADOnlyAuthentication: false
    }
    restrictOutboundNetworkAccess: 'Disabled'
  }
  location: 'westeurope'
  tags: {
  }
  name: 'leerlevels'
}

@description('Generated from /subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/resourceGroups/LeerLevels/providers/Microsoft.Sql/servers/leerlevels/databases/LeerLevels')
resource LeerLevels 'Microsoft.Sql/servers/databases@2022-05-01-preview' = {
  sku: {
    name: 'GP_S_Gen5'
    tier: 'GeneralPurpose'
    family: 'Gen5'
    capacity: 1
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
    maxSizeBytes: 34359738368
    catalogCollation: 'SQL_Latin1_General_CP1_CI_AS'
    zoneRedundant: false
    readScale: 'Disabled'
    autoPauseDelay: 60
    requestedBackupStorageRedundancy: 'Geo'
    minCapacity: '0.5000'
    maintenanceConfigurationId: '/subscriptions/77165f22-39ac-48c3-9768-d70dd3de7f63/providers/Microsoft.Maintenance/publicMaintenanceConfigurations/SQL_Default'
    isLedgerOn: false
  }
  location: 'westeurope'
  tags: {
  }
  name: 'leerlevels/LeerLevels'
}
