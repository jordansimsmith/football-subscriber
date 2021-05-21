import React from 'react';
import Select from 'react-select';
import { useQuery } from 'react-query';
import { ICompetition, IOption } from '../types/types';
import { useTheme } from '@chakra-ui/system';

interface CompetitionSelectProps {
  value: IOption;
  onChange: (option: IOption) => void;
}

export const CompetitionSelect: React.FC<CompetitionSelectProps> = ({
  value,
  onChange,
}) => {
  const { data, isLoading } = useQuery<ICompetition[]>(
    'competitions',
    async () => {
      const url = `${process.env.NEXT_PUBLIC_SERVER_BASE}/competitions`;
      const res = await fetch(url);
      const data = await res.json();
      return data;
    },
  );

  const options = React.useMemo(() => {
    if (!data) {
      return [];
    }

    return data.map((o) => ({
      value: o.id,
      label: o.name,
    }));
  }, [data]);

  const theme = useTheme();

  return (
    <Select
      placeholder="Select a competition"
      value={value}
      onChange={onChange}
      options={options}
      isLoading={isLoading}
      instanceId="competition-select"
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
