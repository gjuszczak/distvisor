import { MsalGuardConfiguration, MsalInterceptorConfiguration } from "@azure/msal-angular";
import { Configuration, IPublicClientApplication, LogLevel, PublicClientApplication } from "@azure/msal-browser";
import { ClientConfiguration } from "src/app/api/models";
import { environment } from "src/environments/environment";

export function MsalInstanceFactory(clientConfig: ClientConfiguration): IPublicClientApplication {
    const config = (clientConfig.msal || {}) as Configuration;
    // if (!environment.production) {
    //     config.system = {
    //         loggerOptions: {
    //             loggerCallback(logLevel: LogLevel, message: string) {
    //                 console.log(message);
    //             },
    //             logLevel: LogLevel.Verbose,
    //             piiLoggingEnabled: false
    //         }
    //     }
    // }
    return new PublicClientApplication(config);
}

export function MsalGuardConfigFactory(clientConfig: ClientConfiguration): MsalGuardConfiguration {
    const config = (clientConfig.msalGuard || {}) as MsalGuardConfiguration;
    const protectedResources = clientConfig.msalProtectedResources || [];
    config.authRequest = {
        scopes: new Array<string>().concat(...new Set(protectedResources.map(res => res.scopes || [])))
    };
    return config;
}

export function MsalInterceptorConfigFactory(clientConfig: ClientConfiguration): MsalInterceptorConfiguration {
    const config = (clientConfig.msalInterceptor || {}) as MsalInterceptorConfiguration;
    const protectedResources = clientConfig.msalProtectedResources || [];
    const protectedResourceMap = new Map<string, Array<string>>();
    protectedResources.forEach(res => {
        const urls = res.urls || [];
        const scopes = res.scopes || [];
        if (scopes.length === 0) {
            return;
        }
        urls.forEach(url => {
            protectedResourceMap.set(url, scopes);
        })
    });
    config.protectedResourceMap = protectedResourceMap;
    return config;
}
