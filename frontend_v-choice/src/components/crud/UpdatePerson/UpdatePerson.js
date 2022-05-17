import React, { useState } from 'react'
import {
	createStyles,
	makeStyles,
	Button,
	Dialog,
	DialogActions,
	DialogContent,
	DialogTitle,
	DialogContentText,
	TextField,
} from '@material-ui/core'
import CreateIcon from '@material-ui/icons/Create'

import MyAlerter from '../../atoms/MyAlerter/MyAlerter'


const useStyles = makeStyles((theme) => createStyles({
	item: {
		margin: theme.spacing(2, 0)
	},
	mainButton: {
		margin: theme.spacing(1)
	},
}));

function UpdatePerson(props) {
	const classes = useStyles();

	const [open, setOpen] = useState(false);
	const [error, setError] = useState(null);
	const [msg, setMsg] = useState(null);
	const [fullName, setFullName] = useState(props.fullName);
	const [image, setImage] = useState(null);

	const handleOpenDialog = () => {
		setOpen(true);
	};

	const handleCloseDialog = () => {
		setOpen(false);
		setError(null);
		setMsg(null);
		setImage(null);
	};

	const handleSubmit = () => {
		if (fullName) {
			const formData = new FormData();
			formData.append("fullName", fullName);
			formData.append("photo", image);

			fetch(`https://localhost:5001/api/person/${props.id}`, {
				method: 'PUT',
				body: formData
			})
				.then(async (response) => {
					if (response.status === 401) {
						setError("Недостаточно прав для выполнения операции");
						setMsg(null);
					}
					if (response.status === 400) {
						setError("Проверьте корректность введенных данных");
						setMsg(null);
					}
					if (response.status === 200) {
						const data = await response.json();
						let personData = { fullName: fullName, photoPath: null };

						if (image === null || (data && image !== null && data.photoPath !== null)) {
							personData.photoPath = image === null ? null : data.photoPath;
							setError(null);
							setMsg("Знаменитость успешно обновлена");
						}
						else {
							setError(null);
							setMsg("Знаменитость обновлена без изменения фото. Попробуйте изменить фото позже");
						}
						props.onUpdate();
					}
				});
		}
		else {
			setError("Введите ФИО");
		}
	};

	const handleChangeFullName = (event) => {
		setFullName(event.target.value);
	};

	const handleChangeImage = (event) => {
		setImage(event.target.files[0]);
	}
	return (
		<>
			<Button color="primary" onClick={handleOpenDialog}>
				<CreateIcon />
			</Button>
			<Dialog open={open} onClose={handleCloseDialog} aria-labelledby="form-dialog-title">
				<DialogTitle id="form-dialog-title">Обновить знаменитость</DialogTitle>
				<DialogContent>
					<MyAlerter msg={msg} error={error} />
					<DialogContentText>
						Заполните информацию о знаменитости
					</DialogContentText>
					<TextField
						autoFocus
						margin="dense"
						id="fullName"
						label="ФИО"
						type="input"
						className={classes.item}
						onChange={handleChangeFullName}
						value={fullName}
						fullWidth
					/>
					<input type="file" className={classes.item} onChange={handleChangeImage} />
				</DialogContent>
				<DialogActions>
					<Button onClick={handleCloseDialog} color="primary">
						Закрыть
					</Button>
					<Button onClick={handleSubmit} color="primary">
						Обновить
					</Button>
				</DialogActions>
			</Dialog>
		</>
	)
}

export default UpdatePerson