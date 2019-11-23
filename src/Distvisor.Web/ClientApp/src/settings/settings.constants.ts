let applicationPaths: ApplicationPathsType = {
  ApiGetUpdates: `api/settings/updates`,
  ApiUpdate: `api/settings/update`,
};

interface ApplicationPathsType {
  readonly ApiGetUpdates: string;
  readonly ApiUpdate: string;
}

export const ApplicationPaths: ApplicationPathsType = applicationPaths;
