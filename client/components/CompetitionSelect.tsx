import { Select } from '@chakra-ui/select';
import React from 'react';
import { useQuery } from 'react-query';
import { ICompetition } from '../types/types';

interface CompetitionSelectProps {
  value: number;
  onChange: (competitionId: number) => void;
}

export const CompetitionSelect: React.FC<CompetitionSelectProps> = ({
  value,
  onChange,
}) => {
  const { data } = useQuery<ICompetition[]>(
    'competitions',
    async () => {
      const res = await fetch('http://localhost:5000/competitions');
      const data = await res.json();
      return data;
    },
    { initialData: [] },
  );

  const handleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const competitionId = Number(e.target.value);
    onChange(competitionId);
  };

  const options = React.useMemo(() => {
    return data.map((o) => (
      <option value={o.id} key={o.id}>
        {o.name}
      </option>
    ));
  }, [data]);

  return (
    <Select
      placeholder="Select a competition"
      value={value ?? ''}
      onChange={handleChange}
    >
      {options}
    </Select>
  );
};
