let applicationPaths: ApplicationPathsType = {
  ApiGetUpdateParams: `api/settings/update-params`,
  ApiUpdate: `api/settings/update`,
};

interface ApplicationPathsType {
  readonly ApiGetUpdateParams: string;
  readonly ApiUpdate: string;
}

export const ApplicationPaths: ApplicationPathsType = applicationPaths;
