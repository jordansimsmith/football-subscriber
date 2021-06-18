import { ArrowLeftIcon, ArrowRightIcon, RepeatIcon } from '@chakra-ui/icons';
import {
  Box,
  Button,
  Center,
  SimpleGrid,
  Text,
  useMediaQuery,
} from '@chakra-ui/react';
import React from 'react';

interface FixtureControlsProps {
  disabled: boolean;
  fromDate: Date;
  toDate: Date;
  onFromDateChange: (date: Date) => void;
  onToDateChange: (date: Date) => void;
}

export const FixtureControls: React.FC<FixtureControlsProps> = ({
  disabled,
  fromDate,
  toDate,
  onFromDateChange,
  onToDateChange,
}) => {
  const handleDateChange = (dateChange: number) => () => {
    const newFromDate = new Date(fromDate);
    const newToDate = new Date(toDate);

    newFromDate.setDate(newFromDate.getDate() + dateChange);
    newToDate.setDate(newToDate.getDate() + dateChange);

    onFromDateChange(newFromDate);
    onToDateChange(newToDate);
  };

  const handlePreviousRound = handleDateChange(-7);
  const handleNextRound = handleDateChange(7);

  const handleResetRound = () => {
    const newFromDate = new Date();
    const newToDate = new Date();

    newToDate.setDate(newToDate.getDate() + 7);

    onFromDateChange(newFromDate);
    onToDateChange(newToDate);
  };

  const [isMediumScreen] = useMediaQuery('(min-width: 600px)');
  console.log(isMediumScreen);

  return (
    <Box>
      <Center padding="20px">
        <Text color="gray.600">
          Fixtures between {fromDate.toDateString()} - {toDate.toDateString()}
        </Text>
      </Center>

      <Box>
        <SimpleGrid columns={isMediumScreen ? 3 : 1} gap="10px">
          <Button
            disabled={disabled}
            leftIcon={<ArrowLeftIcon />}
            onClick={handlePreviousRound}
          >
            Previous Round
          </Button>
          <Button
            leftIcon={<RepeatIcon />}
            onClick={handleResetRound}
            disabled={disabled}
          >
            Reset
          </Button>
          <Button
            disabled={disabled}
            rightIcon={<ArrowRightIcon />}
            onClick={handleNextRound}
          >
            Next Round
          </Button>
        </SimpleGrid>
      </Box>
    </Box>
  );
};
