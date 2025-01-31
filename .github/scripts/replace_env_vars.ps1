# Replace placeholders in appsettings.json with environment variables
param (
    [string]$appSettingsPath = "$env:GITHUB_WORKSPACE/appsettings.json"
)

# Read the appsettings.json file
$appSettings = Get-Content -Path $appSettingsPath -Raw

# Replace placeholders with environment variables
$appSettings = $appSettings -replace '\$\{AUTH0_DOMAIN\}', $env:AUTH0_DOMAIN
$appSettings = $appSettings -replace '\$\{AUTH0_CLIENT_ID\}', $env:AUTH0_CLIENT_ID
$appSettings = $appSettings -replace '\$\{AUTH0_AUDIENCE\}', $env:AUTH0_AUDIENCE
$appSettings = $appSettings -replace '\$\{AUTH0_REDIRECT_URI\}', $env:AUTH0_REDIRECT_URI
$appSettings = $appSettings -replace '\$\{AUTH0_POST_LOGOUT_REDIRECT_URI\}', $env:AUTH0_POST_LOGOUT_REDIRECT_URI

$appSettings = $appSettings -replace '\$\{SMTP_SERVER\}', $env:SMTP_SERVER
$appSettings = $appSettings -replace '\$\{SMTP_PORT\}', $env:SMTP_PORT
$appSettings = $appSettings -replace '\$\{SMTP_SENDER_NAME\}', $env:SMTP_SENDER_NAME
$appSettings = $appSettings -replace '\$\{SMTP_USERNAME\}', $env:SMTP_USERNAME
$appSettings = $appSettings -replace '\$\{SMTP_PASSWORD\}', $env:SMTP_PASSWORD
$appSettings = $appSettings -replace '\$\{SMTP_RECEIVER\}', $env:SMTP_RECEIVER
$appSettings = $appSettings -replace '\$\{SMTP_RECEIVER_NAME\}', $env:SMTP_RECEIVER_NAME
$appSettings = $appSettings -replace '\$\{SMTP_USE_SSL\}', $env:SMTP_USE_SSL

$appSettings = $appSettings -replace '\$\{FEATURE_ROOM_ACTIONS_DELETE\}', $env:FEATURE_ROOM_ACTIONS_DELETE
$appSettings = $appSettings -replace '\$\{FEATURE_ROOM_ACTIONS_EDIT\}', $env:FEATURE_ROOM_ACTIONS_EDIT
$appSettings = $appSettings -replace '\$\{FEATURE_ROOM_ACTIONS_VIEW\}', $env:FEATURE_ROOM_ACTIONS_VIEW

$appSettings = $appSettings -replace '\$\{FEATURE_PACKAGE_ACTIONS_DELETE\}', $env:FEATURE_PACKAGE_ACTIONS_DELETE
$appSettings = $appSettings -replace '\$\{FEATURE_PACKAGE_ACTIONS_EDIT\}', $env:FEATURE_PACKAGE_ACTIONS_EDIT
$appSettings = $appSettings -replace '\$\{FEATURE_PACKAGE_ACTIONS_VIEW\}', $env:FEATURE_PACKAGE_ACTIONS_VIEW

$appSettings = $appSettings -replace '\$\{COMPANY_NAME\}', $env:COMPANY_NAME
$appSettings = $appSettings -replace '\$\{COMPANY_ADDRESS\}', $env:COMPANY_ADDRESS
$appSettings = $appSettings -replace '\$\{COMPANY_CITY\}', $env:COMPANY_CITY
$appSettings = $appSettings -replace '\$\{COMPANY_MOBILE_NO\}', $env:COMPANY_MOBILE_NO
$appSettings = $appSettings -replace '\$\{COMPANY_ALTERNATE_MOBILE_NO\}', $env:COMPANY_ALTERNATE_MOBILE_NO
$appSettings = $appSettings -replace '\$\{COMPANY_EMAIL_ID\}', $env:COMPANY_EMAIL_ID
$appSettings = $appSettings -replace '\$\{COMPANY_ALTERNATE_EMAIL_ID\}', $env:COMPANY_ALTERNATE_EMAIL_ID

# Write the modified appsettings.json back to the file
Set-Content -Path $appSettingsPath -Value $appSettings