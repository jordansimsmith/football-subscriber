import React from 'react';
import Select from 'react-select';
import { useQuery } from 'react-query';
import { useTheme } from '@chakra-ui/system';
import { ITeam, IOption } from './../types/types';

interface TeamSelectProps {
  value: IOption;
  onChange: (option: IOption) => void;
}

export const TeamSelect = ({
  value,
  onChange,
}: TeamSelectProps): JSX.Element => {
  const { data, isLoading } = useQuery<ITeam[]>('teams', async () => {
    const url = `${process.env.NEXT_PUBLIC_SERVER_BASE}/teams`;
    const res = await fetch(url);
    const data = await res.json();
    return data;
  });

  const options = React.useMemo(() => {
    if (!data) {
      return [];
    }

    return data.map((o, i) => ({
      value: i,
      label: o.name,
    }));
  }, [data]);

  const theme = useTheme();

  return (
    <Select
      placeholder="Select a team"
      value={value}
      onChange={onChange}
      options={options}
      isLoading={isLoading}
      instanceId="team-select"
      theme={(t) => ({
        ...t,
        colors: {
          ...t.colors,
          primary: theme.colors.teal['300'],
          primary75: theme.colors.teal['200'],
          primary50: theme.colors.teal['100'],
          primary25: theme.colors.teal['50'],
        },
      })}
      styles={{
        control: (provided, state) => ({
          ...provided,
          borderColor: state.isFocused
            ? provided.borderColor
            : theme.colors.gray['200'],
        }),
      }}
    />
  );
};
