import React, { useState } from 'react'
import { createStyles, makeStyles, useTheme, Box, Button, Collapse, Grid, MobileStepper, Typography } from '@material-ui/core'
import ExpandLessIcon from '@material-ui/icons/ExpandLess'
import ExpandMoreIcon from '@material-ui/icons/ExpandMore'
import KeyboardArrowLeft from '@material-ui/icons/KeyboardArrowLeft'
import KeyboardArrowRight from '@material-ui/icons/KeyboardArrowRight'
import SwipeableViews from "react-swipeable-views"
import { autoPlay } from "react-swipeable-views-utils"

import PersonCard from "../../card&tiles/PersonCard/PersonCard"

const useStyles = makeStyles((theme) => createStyles({
	container: {
		margin: theme.spacing(2, 0),
	},
	header: {
		margin: theme.spacing(2, 0),
		display: 'flex',
		justifyContent: 'space-between',
		alignItems: 'center',
	},
	grid: {
		display: 'flex',
		justifyContent: 'center',
	},
	panel: {
		margin: theme.spacing(2, 0),
	},
}));

const AutoPlaySwipeableViews = autoPlay(SwipeableViews);

const getData = (persons) => {
	let arr = [];
	for (let i = 0; i < persons.length; i = i + 3) {
		arr.push(
			[...persons].splice(i, 3).map((person) => {
				return {
					role: person.role,
					fullName: person.fullName,
					photoPath: person.photoPath
				};
			})
		);
	}
	return arr;
};

function PersonsCarousel(props) {
	const theme = useTheme();
	const classes = useStyles();
	const [open, setOpen] = useState(false);
	const [activeStep, setActiveStep] = useState(0);
	const persons = [
		...props.directors.map(d => { return { ...d, role: "режиссёр" } }),
		...props.cast.map(a => { return { ...a, role: "актёр" } })
	];
	const data = getData(persons);
	const maxSteps = data.length;

	const handleNext = () => {
		const next = activeStep < maxSteps - 1 ? activeStep + 1 : 0;
		setActiveStep(next);
	};

	const handleBack = () => {
		const prev = activeStep > 0 ? activeStep - 1 : maxSteps - 1;
		setActiveStep(prev);
	};

	const handleStepChange = (step) => {
		setActiveStep(step);
	};

	return (
		<Box className={classes.container}>
			<Box className={classes.header}>
				<Typography variant='h5'>Участие знаменитостей</Typography>
				<Button onClick={() => setOpen(!open)} size="small">
					{open ? <ExpandLessIcon fontSize="small" /> : <ExpandMoreIcon fontSize="small" />}
					{open ? "Свернуть" : "Развернуть"}
				</Button>
			</Box>
			<Collapse in={open} timeout="auto">
				<AutoPlaySwipeableViews
					axis={theme.direction === "rtl" ? "x-reverse" : "x"}
					index={activeStep}
					onChangeIndex={handleStepChange}
					enableMouseEvents
				>
					{data.map((step, index) => (
						<Box key={index}>
							<Grid container className={classes.grid} spacing={2}>
								{
									step.map(item => (
										<Grid key={item.fullName} item>
											<PersonCard {...item} />
										</Grid>
									))
								}
							</Grid>
						</Box>
					))}
				</AutoPlaySwipeableViews>
				<MobileStepper
					className={classes.panel}
					steps={maxSteps}
					position="static"
					activeStep={activeStep}
					nextButton={
						<Button size="small" onClick={handleNext}>
							Вперед
							{theme.direction === "rtl" ? (
								<KeyboardArrowLeft />
							) : (
								<KeyboardArrowRight />
							)}
						</Button>
					}
					backButton={
						<Button size="small" onClick={handleBack}>
							{theme.direction === "rtl" ? (
								<KeyboardArrowRight />
							) : (
								<KeyboardArrowLeft />
							)}
							Назад
						</Button>
					}
				/>
			</Collapse>
		</Box >
	);
}

export default PersonsCarousel;
