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

interface SubscriptionFormProps {
  apiToken: string;
}

export const SubscriptionForm: React.FC<SubscriptionFormProps> = ({
  apiToken,
}) => {
  const [team, setTeam] = React.useState<IOption>();

  const queryClient = useQueryClient();

  const { mutate, isLoading, isError, isSuccess } = useMutation(
    async (teamId: number) => {
      const res = await fetch('http://localhost:5000/subscriptions', {
        method: 'POST',
        headers: {
          Authorization: `Bearer ${apiToken}`,
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({ teamId }),
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
        onClick={() => mutate(team.value)}
        display="block"
      >
        Submit
      </Button>
    </Box>
  );
};
