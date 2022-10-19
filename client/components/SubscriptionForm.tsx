import React from 'react';
import { useMutation, useQueryClient } from 'react-query';
import {
  Box,
  Button,
  Alert,
  AlertIcon,
  AlertDescription,
  Stack,
} from '@chakra-ui/react';
import { IOption } from './../types/types';
import { TeamSelect } from './TeamSelect';
import { getAccessToken } from '../lib/api';

export const SubscriptionForm = (): JSX.Element => {
  const [team, setTeam] = React.useState<IOption>();

  const queryClient = useQueryClient();

  const { mutate, isLoading, isError, isSuccess } = useMutation(
    async (teamName: string) => {
      const accessToken = await getAccessToken();

      const url = `${process.env.NEXT_PUBLIC_SERVER_BASE}/subscriptions`;
      const res = await fetch(url, {
        method: 'POST',
        headers: {
          Authorization: `Bearer ${accessToken}`,
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ teamName }),
      });
      if (!res.ok) {
        throw new Error(await res.text());
      }
    },
    {
      onSuccess: () => {
        queryClient.invalidateQueries('subscriptions');
      },
    },
  );

  return (
    <Box>
      <Stack marginTop="10px" direction="column">
        {isError && (
          <Alert status="error">
            <AlertIcon />
            <AlertDescription>
              There was an error creating your subscription
            </AlertDescription>
          </Alert>
        )}

        {isSuccess && (
          <Alert status="success">
            <AlertIcon />
            <AlertDescription>Subscription created</AlertDescription>
          </Alert>
        )}

        <TeamSelect value={team} onChange={setTeam} />
      </Stack>

      <Button
        marginTop="10px"
        colorScheme="teal"
        variant="solid"
        disabled={!team}
        isLoading={isLoading}
        onClick={() => mutate(team.label)}
        display="block"
      >
        Submit
      </Button>
    </Box>
  );
};
