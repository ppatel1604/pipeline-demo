Param(
    [Parameter(Mandatory = $true)]
    [string]
    $WebAppUrl,

    [Parameter(Mandatory = $false)]
    [string]
    $Endpoint = "weatherforecast",

    [Parameter(Mandatory = $false)]
    [int]
    $ExpectedReponseCount = 5
)

if (-not ([uri]::IsWellFormedUriString($WebAppUrl, 'Absolute') -and ([uri] $WebAppUrl).Scheme -in 'http', 'https')) {
    Write-Error "Please provide correct webapp url";
    Return;
}

$webUri = "{0}/{1}" -f $WebAppUrl, $Endpoint;

Write-Verbose ("The uri for the request will be <{0}>" -f $webUri);

try {
    for ($i = 1; $i -lt 10; $i++) {
        $response = Invoke-WebRequest -Uri $webUri -Method Get

        $content = $response.Content | ConvertFrom-Json;

        Write-Verbose "The request is compeleted"

        $statusCode = $response.StatusCode;
        $responseCount = $content.Count;

        if ($statusCode -eq "200" -and $responseCount -eq $ExpectedReponseCount) {
            Write-Host "Healthcheck is successful";
            Write-Verbose "Going on sleep for 1 second for cool down period";
            Start-Sleep -Seconds 1
        }
        else {
            Write-Host ("Response resulted in status code {0} with the response count {1}" -f $statusCode, $responseCount);
            Write-Error "Healthcheck resutls in unexpected status code or response";
        }
    }
}
catch {
    Write-Error "Unexpected error occured while performing the health check";
}