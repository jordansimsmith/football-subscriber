import React from 'react';
import { useQuery } from 'react-query';

export const CompetitionSelect: React.FC<{}> = () => {
  const { data } = useQuery('competitions', async () => {
    const res = await fetch('http://localhost:5000/competitions');
    const data = await res.json();
    return data;
  });

  return <pre>{JSON.stringify(data, null, 2)}</pre>;
};
